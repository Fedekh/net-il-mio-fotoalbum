Multi-user platform where each guest can:

- check all the photos in the database
- search for a photo filtering it by name
- see photo details
- send messages to the author of the photo

The public homepage contains PAGING and SEARCH BAR

A Photo contains at least the following properties:

- Title
- Description
- Image (which can be loaded from file or url; file precedence)
- Visible or not to guests
- Associated categories

A Photo can have multiple categories. A category can belong to multiple Photos

The platform provides some basic categories, common to all Admins

If the guest wants, he can register and become an Admin.

Each Admin can:

- See all HIS photos
- You can filter them by searching for them in a searchbar
- See individual details of each HIS photo
- Create a Photo
- Edit your photo
- Delete a photo of YOURS

- See all the categories made available by the platform
- Create your own custom category
- Edit your own custom category
- Delete your custom category

The admin Dashboard will be equipped with PAGING and SEARCH BAR
For each CRUD operation there are all the relevant validations

The SUPERADMIN figure will be able to act on the VISIBILITY property of the photos, to show them or not on the public homepage
