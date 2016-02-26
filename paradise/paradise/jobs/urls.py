__author__ = 'swapneshchaubal'

from django.conf.urls import patterns, include, url

urlpatterns = patterns('jobs.views',
                       url(r'^add_user$', 'new_user', name='jobs_invite')
                       )