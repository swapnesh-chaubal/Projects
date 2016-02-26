from __future__ import unicode_literals
from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Candidate(models.Model):
    user = models.OneToOneField(User)
    first_name = models.CharField(max_length=20, blank=True)
    middle_name = models.CharField(max_length=20, blank=True)
    last_name = models.CharField(max_length=20, blank=True)
    years_of_experience = models.IntegerField()
    education = models.CharField(max_length=20)
    linked_in = models.CharField(max_length=25, blank=True)
    resume = models.FileField()
    skills = models.CharField(max_length=300)

    def __str__(self):
        return "{0} {1}".format(self.first_name, self.middle_name, self.last_name)

class Company(models.Model):
    name = models.CharField(max_length=20)
    info = models.CharField(max_length=100, blank=True)

    def __str__(self):
        return "{0}".format(self.name)

class Job(models.Model):
    name = models.CharField(max_length=20)
    location = models.CharField(max_length=20)
    description = models.CharField(max_length=300, blank=True)
    company = models.ForeignKey(Company)
    years_of_experience = models.IntegerField()
    education = models.CharField(max_length=20)
    skills = models.CharField(max_length=300)

    def __str__(self):
        return "{0}, {1}".format(self.name, self.location)
'''
class Skill(models.Model):
    name = models.CharField(max_length=20)
    job = models.ForeignKey(Job)
    candidate = models.ForeignKey(Candidate)

    def __str__(self):
        return "{0}".format(self.name)
'''