Requirements:
All requirements for this project are saved in requirements.txt. The virtualenv can be recreated using that.

Steps:
1. Create tables
    Tables can be created using the command:
        python manage.py initdb

2. Ingest data:
    Note: The filename "National_Downloadable_file" has been harcoded at the moment.
    Data can be ingested using the command:
        python manage.py ingest

3. Delete tables
    Tables can be deleted using the command:
        python manage.py dropdb

4. Run server
    Server can be started using the command:
        python manage.py runserver


Pagination:
    The number of records per page can be changed by setting the PAGE_SIZE variable
