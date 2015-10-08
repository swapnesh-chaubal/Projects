from provider_search import db

specialties = db.Table('doctor_specialty',
    db.Column('doctor_id', db.Integer, db.ForeignKey('doctor.id')),
    db.Column('specialty_id', db.Integer, db.ForeignKey('specialty.id'))
    )

# we have only 3 Tables. One consideration was to have a separate table for addresses (1 to many relationship between doctor and address) but that would have 
# required more querying and joins in order to check if a doctor has multiple addresses. An easier way is 
# to add another entry for the doctor with a different address. Since the probablity of having multiple
# addreses for a doctor is not very high, a lot of duplicate rows wont be created and this helps minimize
# the complexity

class Doctor(db.Model):
    #__tablename__ = 'doctor'
    id = db.Column(db.Integer, primary_key=True)
    last_name = db.Column(db.Text, nullable=False)
    first_name = db.Column(db.Text, nullable=False)
    address_line1 = db.Column(db.Text)
    zipcode = db.Column(db.Text, nullable=False, index=True)
    org_id = db.Column(db.Integer, db.ForeignKey('organization.id'))
    _doctor_specialty = db.relationship('Specialty', secondary=specialties, 
    	backref=db.backref('doctors', lazy='dynamic'))

class Organization(db.Model):
    #__tablename__ = 'organization'
    id = db.Column(db.Integer, primary_key=True)
    org_legal_name = db.Column(db.Text, nullable=False)
    doctors = db.relationship('Doctor', backref='Organization', lazy='dynamic')

class Specialty(db.Model):
	id = db.Column(db.Integer, primary_key=True)
	specialty_name = db.Column(db.Text, nullable=False, index=True)
