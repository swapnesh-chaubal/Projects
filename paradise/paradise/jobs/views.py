from django.shortcuts import render
from django.contrib.auth.decorators import login_required
from django.http import HttpResponse
from .models import Candidate
from .forms import CandidateForm

# Create your views here.
#@login_required
def home(request):
    return render(request, "jobs/main.html")

def add_user(request):
    if request.method == 'POST':
        form = CandidateForm(data=request.POST)
        if form.is_valid():
            form.save()
            return redirect('/')
    else:
        form = CandidateForm()
    return render(request, "jobs/add_candidate.html", {'form': form})