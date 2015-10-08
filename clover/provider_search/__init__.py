from flask import Flask, render_template, url_for, request, redirect, session
from flask_sqlalchemy import SQLAlchemy
import os
from forms import ProviderSearchForm

#from models import Specialty, Doctor, Organization

basedir = os.path.abspath(os.path.dirname(__file__))

app = Flask(__name__)
app.config['SECRET_KEY'] = '\x81Q\x01\x17\xae\xb9Oq\x14oS\xf3>c6\x9fS\xbcmX\xa9\x00n='
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///' + os.path.join(basedir, 'providers.db')

db = SQLAlchemy(app)
import models
import views