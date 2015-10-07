import pdb
import os
from models import Doctor, Organization, Specialty
from provider_search import db
 
class FileParser():
    def __init__(self):
        self.specialty_key = 'specialty'
    	self.column_dict = dict()
    	self.columns_to_care = ("Last Name", "First Name", "Primary specialty", "Secondary specialty 1",
            "Secondary specialty 3", "Secondary specialty 2", "Secondary specialty 4", "Organization legal name", 
		    "Line 1 Street Address", "Line 2 Street Address", "City", "State", "Zip Code")
        
    def fill_column_numbers(self, line):
        if ',' in line:

            columns = line.split(',')
            for i in range(0, len(columns)):
                column_name = columns[i].strip()
                if column_name in self.columns_to_care:
                    if 'specialty' in column_name:   # its a specialty column
                        if self.specialty_key in self.column_dict:
                            specialties = self.column_dict[self.specialty_key]
                            specialties.append(i) 
                        else:
                            specialties = list()
                            specialties.append(i)
                            self.column_dict[self.specialty_key] = specialties
                    else:
                        # we store the column name and its column index
                        self.column_dict[column_name] = i
        else:
                print("Invalid CSV file, seperator should be a comma ',': {0}".format(line)) 

    	self.columns_to_care = ("Last Name", "First Name", "Primary specialty", "Secondary specialty 1",
            "Secondary specialty 3", "Secondary specialty 2", "Secondary specialty 4", "Organization legal name", 
		    "Line 1 Street Address", "Line 2 Street Address", "City", "State", "Zip Code")
 
    def get_doctor(self, columns):
        try:
            doctor = Doctor()
            last_name = columns[self.column_dict['Last Name']].strip()
            if columns[last_name != '']:
                doctor.last_name = last_name

            first_name = columns[self.column_dict['First Name']].strip()
            if columns[first_name != '']:
                 doctor.first_name = first_name

            address1 = columns[self.column_dict['Line 1 Street Address']].strip()
            if columns[address1 != '']:
                 doctor.address1 = address1

            address2 = columns[self.column_dict['Line 2 Street Address']].strip()
            if columns[address2 != '']:
                doctor.address2 = address2

            zipcode = columns[self.column_dict['Zip Code']].strip()
            if columns[zipcode != '']:
                doctor.zipcode = zipcode

            return doctor 

        except (TypeError, ValueError):
            print ("Error with positioning of values in datafile. Line '{}'".format(line))
            exit(os.EX_DATAERR)
        except Exception as e:
            print(str(e))
            exit(os.EX_SOFTWARE)       

    def get_org(self, columns):
        org_name = columns[self.column_dict['Organization legal name']] 
        if org_name.strip() == '':
            return None

        if org_name not in self.org_names:
            self.org_names[org_name] = 1
            org = Organization(org_legal_name = org_name)
            db.session.add(org)
            db.session.commit()
            org = Organization.query.filter_by(id = org.id).all()[0]
            return org

        # org already exists in the table
        org = Organization.query.filter_by(org_legal_name = org_name).all()[0]
        return org
      
    def get_specialty(self, columns):
        specialties = self.column_dict[self.specialty_key]
        for specialty in specialties:
            specialty_name = columns[specialty]
            if specialty_name.strip() == '':
                continue
            if specialty_name not in self.specialty_names:
                sp = Specialty(specialty_name= specialty_name)
                db.session.add(sp)
                db.session.commit()
                self.specialty_names[specialty_name] = 1
                sp = Specialty.query.filter_by(id=sp.id).all()[0]
                return sp
 
            sp = Specialty.query.filter_by(specialty_name = specialty_name).all()[0]
            return sp

        return None

    def ingest_data(self, datafilename):
        """
        Gets the datafile and formatfile, parses the datafile and returns the data in a usable format.
        """

        if os.path.splitext(datafilename)[1] != '.csv':
            print("Data file needs to be csv: {}".format(datafilename))
            exit(os.EX_DATAERR)

        try:
            data_file = open(datafilename)
            # read the first line to get the indices of the columns
            self.fill_column_numbers(data_file.readline())

            self.org_names = dict()
            self.specialty_names = dict()

            records = 0
            bulk_write_size = 100   # write every 500 records 
            for row in data_file:
                columns = row.split(',')
                state = columns[self.column_dict['State']]
                if state != 'NJ':
                    continue
    
                org = self.get_org(columns)
                specialty = self.get_specialty(columns)
                doctor = self.get_doctor(columns)
                #pdb.set_trace()
                if org is not None:
                    doctor.Organization = org
                else:
                    doctor.Organization = None
                
                db.session.add(doctor)
                db.session.commit()

                doctor = Doctor.query.filter_by(id = doctor.id).all()[0]
                if specialty is not None:
                    specialty.doctors.append(doctor)
                    db.session.add(specialty)
                    db.session.commit()
            
                records += 1
                if records % bulk_write_size == 0:
                    #db.session.commit()
                    break

            #db.session.commit()

            print "{} total record(s) of doctors added to the database".format(records)
    
        except (TypeError, ValueError):
            print ("Error with positioning of values in datafile. Line '{}'".format(line))
            exit(os.EX_DATAERR)
        except Exception as e:
            print(str(e))
            import traceback
            print(traceback.format_exc())
            exit(os.EX_SOFTWARE)       


parser = FileParser()
orgs_specialties = parser.ingest_data("National_Downloadable_File.csv") 

