from flask_wtf import Form
from wtforms.fields import StringField
from wtforms.validators import DataRequired

class ProviderSearchForm(Form):
    zipcode = StringField('zipcode', validators=[DataRequired()])
    specialty = StringField('specialty', validators=[DataRequired()])
