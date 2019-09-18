docker-compose down --rmi local

cd Sprinters.Webshop.BFF\Sprinters.Webshop.BFF
dotnet publish -f "netcoreapp2.2" -r "linux-x64" -c debug -o "obj\docker\publish"
cd ..\..\

cd Sprinters.BestellingBeheer\Sprinters.BestellingBeheer\
dotnet publish -f "netcoreapp2.2" -r "linux-x64" -c debug -o "obj\docker\publish"
cd ..\..\

cd Sprinters.KlantBeheer\Sprinters.KlantBeheer\
dotnet publish -f "netcoreapp2.2" -r "linux-x64" -c debug -o "obj\docker\publish"
cd ..\..\

cd Sprinters.Authenticatie\Sprinters.Authenticatie\
dotnet publish -f "netcoreapp2.2" -r "linux-x64" -c debug -o "obj\docker\publish"
cd ..\..\

cd Sprinters.Webshop.Angular\
call npm install
call npm run ng build --prod
cd ..\

cd Sprinters.Magazijn.Angular\
call npm install
call npm run ng build --prod
cd ..\

docker-compose up -d

pause