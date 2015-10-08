import os
from models import Doctor, Organization, Specialty
from provider_search import db
from csv import reader
 
class FileParser():
    def __init__(self):
        self.specialty_key = 'specialty'
    	self.column_dict = dict()
    	self.columns_to_care = ("Last Name", "First Name", "Primary specialty", "Secondary specialty 1",
            "Secondary specialty 3", "Secondary specialty 2", "Secondary specialty 4", "Organization legal name", 
		    "Line 1 Street Address", "Line 2 Street Address", "City", "State", "Zip Code")
        
    def fill_column_numbers(self, columns):

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

    def get_doctor(self, columns):
        try:
            doctor = Doctor()
            last_name = columns[self.column_dict['Last Name']].strip()
            if last_name != '':
                doctor.last_name = last_name

            first_name = columns[self.column_dict['First Name']].strip()
            if first_name != '':
                 doctor.first_name = first_name

            address1 = columns[self.column_dict['Line 1 Street Address']].strip()
            if address1 != '':
                 doctor.address_line1 = address1

            zipcode = columns[self.column_dict['Zip Code']].strip()
            if zipcode != '':
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


        orgs = Organization.query.filter_by(org_legal_name = org_name).all()
        if len(orgs) == 0:
            # org doesn't exist in the db
            org = Organization(org_legal_name = org_name)
            db.session.add(org)
            db.session.commit()
            return org

        # org already exists in the table
        return orgs[0]
      
    def add_specialty(self, columns, doctor):
        specialties = self.column_dict[self.specialty_key]
        for specialty in specialties:
            specialty_name = columns[specialty]
            if specialty_name.strip() == '':
                continue
            sps = Specialty.query.filter_by(specialty_name=specialty_name).all()
            
            if len(sps) == 0:
                # specialty doesn't exist in the DB
                sp = Specialty(specialty_name= specialty_name)
                sp.doctors.append(doctor)
                db.session.add(sp)
                db.session.commit()
                continue
 
            sps = sps[0]
            sps.doctors.append(doctor)
            db.session.commit()

    def ingest_data(self, datafilename):
        """
        Gets the datafile and formatfile, parses the datafile and returns the data in a usable format.
        """

        if os.path.splitext(datafilename)[1] != '.csv':
            print("Data file needs to be csv: {}".format(datafilename))
            exit(os.EX_DATAERR)

        data_file = open(datafilename)
        first_row = True
        records = 0
        with open(datafilename, 'r') as csvfile:
                
            columns = reader(csvfile, delimiter=',', quotechar='"')
            for row in columns:
                if first_row:
                    self.fill_column_numbers (row)
                    first_row = False
                    continue
                
                state = row[self.column_dict['State']]
                if state != 'NJ':
                    continue
    
                org = self.get_org(row)
                doctor = self.get_doctor(row)
                if org is not None:
                    doctor.Organization = org
                else:
                    doctor.Organization = None
                
                db.session.add(doctor)
                db.session.commit()
                
                self.add_specialty(row, doctor)
                records += 1
                
                if records % 100 == 0:
                    print("Adding records. {} records added...".format(records))

            print "{} total record(s) of doctors added to the database".format(records)
    

