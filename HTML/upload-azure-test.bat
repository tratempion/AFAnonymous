mkdir "/dist/src" & mkdir "dist/src/assets" & copy "src\assets\afupeo.png"  "dist\src\assets\afupeo.png" & az storage blob delete-batch -s $web --account-name comptestockageprojannuel & az storage blob upload-batch -d $web --account-name comptestockageprojannuel -s dist/

