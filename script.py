from azure.storage.blob import BlockBlobService
from azure.storage.blob import ContentSettings

block_blob_service = BlockBlobService(account_name='storage2349', account_key='I1F+6O9FeF469O5j9QOm8tbZXymXC2Ayo3JwG9dcmfSRlyrgKtbNGgT8Dke9n52BGwlIVscoJDIrAtKlYUjJ1w==')
block_blob_service.create_container('mycontainer')

#Upload the CSV file to Azure cloud
block_blob_service.create_blob_from_path(
    'mycontainer',
    'images/*',
    content_settings=ContentSettings(content_type='image/jpg')
            )

# Check the list of blob
generator = block_blob_service.list_blobs('mycontainer')
for blob in generator:
    print(blob.name)