from provider_search import app, db
from provider_search.models import Doctor, Organization, Specialty
#from models import Doctor, Organization, Specialty
from flask.ext.script import Manager, prompt_bool
from provider_search.ingest import FileParser
import os
basedir = os.path.abspath(os.path.dirname(__file__))

FILENAME = os.path.join(basedir, "provider_search", "National_Downloadable_File.csv")

manager = Manager(app)

@manager.command
def initdb():
	db.create_all()
	print 'Created the DB and tables'

@manager.command
def dropdb():
	if prompt_bool(
		"Are you sure you want to drop all tables?"):

	    db.drop_all()
	    print 'Dropped the DB'

@manager.command
def ingest_data():
	print(FILENAME)
	parser = FileParser()
	(org_names, speciality_names) = parser.get_orgs_and_specialties(FILENAME)
	#import pdb
	#pdb.set_trace()
	for org in org_names:
		o = Organization(org_legal_name = org)
		db.session.add(o)
	db.session.commit()

	for specialty in speciality_names:
		s = Speciality(speciality_name= specialty)
		db.session.add(s)
	db.session.commit()

	print 'Created Organization names and Speciality names'


if __name__ == '__main__':
	manager.run()