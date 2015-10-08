from provider_search import app, db
from flask.ext.script import Manager, prompt_bool
from provider_search.ingest import FileParser
from provider_search.models import Doctor, Organization, Specialty
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
def ingest():
    parser = FileParser()
    parser.ingest_data(FILENAME)

if __name__ == '__main__':
	manager.run()