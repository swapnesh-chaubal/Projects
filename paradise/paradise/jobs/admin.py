from django.contrib import admin

# Register your models here.
from .models import Job, Company, Candidate

#admin.site.register(Skill)
admin.site.register(Job)
admin.site.register(Company)
admin.site.register(Candidate)
