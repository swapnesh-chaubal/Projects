
from flask import Flask, render_template, url_for, request, redirect, session
from flask_sqlalchemy import SQLAlchemy
import os
from forms import ProviderSearchForm

#from models import Specialty, Doctor, Organization
import models
basedir = os.path.abspath(os.path.dirname(__file__))

app = Flask(__name__)
app.config['SECRET_KEY'] = '\x81Q\x01\x17\xae\xb9Oq\x14oS\xf3>c6\x9fS\xbcmX\xa9\x00n='
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///' + os.path.join(basedir, 'providers.db')

db = SQLAlchemy(app)
PAGE_SIZE = 3

@app.route('/results')
@app.route('/results/<int:page>')
def results(page=1):
    
    zipcode = session['zipcode'] 
    specialty = session['specialty'].upper()
    specialties = models.Specialty.query.filter_by(specialty_name = specialty).all()
    if len(specialties) == 0:
        return "No specialties found with the name {}".format(specialty)

    doctors = specialties[0].doctors.filter_by(zipcode = zipcode).order_by(models.Doctor.last_name).paginate(page, PAGE_SIZE, False)
    return render_template('results.html', specialty=specialty, zipcode=zipcode, doctors=doctors)

@app.route('/', methods=['GET', 'POST'])
#@app.route('/index', methods=['GET', 'POST'])
def index():
    form = ProviderSearchForm()
    if form.validate_on_submit():
        session['zipcode'] = form.zipcode.data
        session['specialty'] = form.specialty.data
        return redirect(url_for('results'))
    return render_template('index.html', form=form)  # by default looks in the template dir

@app.errorhandler(404)
def page_not_found(e):
    return render_template('404.html'), 404

@app.errorhandler(500)
def server_error(e):
    print(e)
    return render_template('500.html'), 500


if __name__=='__main__':
    app.run()
