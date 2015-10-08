from flask import Flask, render_template, url_for, redirect, session
from provider_search import app, db
from forms import ProviderSearchForm
from models import Organization, Specialty, Doctor
import datetime
import time

PAGE_SIZE = 20
LOGFILE = "provider_search_log.txt"

@app.route('/results')
@app.route('/results/<int:page>')
def results(page=1):    
    zipcode = session['zipcode'] 
    specialty = session['specialty'].upper()
    specialties = Specialty.query.filter_by(specialty_name = specialty).all()
    if len(specialties) == 0:
        return "No specialties found with the name {}".format(specialty)

    doctors = specialties[0].doctors.filter_by(zipcode = zipcode).order_by(Doctor.last_name).paginate(page, PAGE_SIZE, False)
    return render_template('results.html', specialty=specialty, zipcode=zipcode, doctors=doctors, num_of_docs = len(doctors.items))

@app.route('/', methods=['GET', 'POST'])
def index():
    form = ProviderSearchForm()
    if form.validate_on_submit():
        session['zipcode'] = form.zipcode.data
        session['specialty'] = form.specialty.data
        return redirect(url_for('results'))
    return render_template('index.html', form=form)  # by default looks in the template dir

@app.errorhandler(404)
def page_not_found(e):
    print(e)
    ts = time.time()
    st = datetime.datetime.fromtimestamp(ts).strftime('%Y-%m-%d %H:%M:%S')
    with open(LOGFILE, 'a') as log:
        log.write("{}: {}\n".format(st, str(e)))

    return render_template('404.html'), 404

@app.errorhandler(500)
def server_error(e):
    print(e)
    ts = time.time()
    st = datetime.datetime.fromtimestamp(ts).strftime('%Y-%m-%d %H:%M:%S')
    with open(LOGFILE, 'a') as log:
        log.write("{}: {}\n".format(st, str(e)))

    return render_template('500.html'), 500
