__author__ = 'swapneshchaubal'
from django.forms import ModelForm
from .models import Candidate

class CandidateForm(ModelForm):
    class Meta:
        model = Candidate
        exclude = ['user']
