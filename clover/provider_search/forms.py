from flask_wtf import Form
from wtforms.fields import StringField
from wtforms.validators import DataRequired
import re
class ProviderSearchForm(Form):
    zipcode = StringField('zipcode', validators=[DataRequired()])
    specialty = StringField('specialty', validators=[DataRequired()])

    def validate(self):
        zip_pattern = re.compile("[0-9]*")
        specialty_pattern = re.compile("[A-Za-z/]*")
        
        if self.zipcode.data.strip() == '':
            self.zipcode.errors = "Please enter zipcode"
            return False

        if self.specialty.data.strip() == '':
            self.specialty.errors = "Please enter specialty"
            return False
        
        if len(zip_pattern.match(self.zipcode.data).group(0)) == 0:
            self.zipcode.errors = "Invalid zipcode, zipcode should be numbers"
            return False

        if len(specialty_pattern.match(self.specialty.data).group(0)) == 0:
            self.specialty.errors = ("Invalid specialty, numbers or symbols aren't allowed")
            return False

        if not Form.validate(self):
            return False

        try:
            zip = long(self.zipcode.data.strip())
            return True
        except Exception as e:
            return False
