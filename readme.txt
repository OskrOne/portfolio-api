## Start DynamoDB locally
## Directory: /c/Data/buffett/dynamodb
java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -sharedDb

## Create tables
aws dynamodb create-table --table-name Portfolio \
						  --attribute-definitions AttributeName=ContractId,AttributeType=S \
						  --key-schema AttributeName=ContractId,KeyType=HASH \
						  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
						  --endpoint-url http://localhost:8000
aws dynamodb create-table --table-name Assets \
						  --attribute-definitions AttributeName=ContractId,AttributeType=S AttributeName=InstrumentId,AttributeType=S \
						  --key-schema AttributeName=ContractId,KeyType=HASH AttributeName=InstrumentId,KeyType=RANGE \
						  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
						  --endpoint-url http://localhost:8000
aws dynamodb create-table --table-name PortfolioEvents \
						  --attribute-definitions AttributeName=ContractId,AttributeType=S AttributeName=TimeStamp,AttributeType=S \
						  --key-schema AttributeName=ContractId,KeyType=HASH AttributeName=TimeStamp,KeyType=RANGE \
						  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
						  --endpoint-url http://localhost:8000


## Putting new items
aws dynamodb put-item --table-name Portfolio \
					  --item '{"ContractId": {"S": "A100"}, "BuyingPower": {"N": "10000"}}' \
					  --endpoint-url http://localhost:8000
aws dynamodb put-item --table-name Assets \
                      --item '{"ContractId": {"S": "A100"}, "InstrumentId": {"S": "1003232"}, "Quantity": {"N": "10"}, "AveragePrice": {"N": "1000"}, "LastPrice": {"N": "1000"}, "ClosePrice": {"N": "1000"} }' --endpoint-url http://localhost:8000

aws dynamodb get-item --table-name Portfolio --key '{"ContractId": {"S": "A100"}}' --endpoint-url http://localhost:8000
aws dynamodb get-item --table-name Portfolio --key '{"ContractId": {"S": "A100"}}' --endpoint-url http://localhost:8000

aws dynamodb update-item --table-name Assets --key '{"ContractId": {"S": "A100"}, "InstrumentId": {"N": "1003232"}}' --endpoint-url http://localhost:8000


## Using Docker
## Start DynamoDB
docker run -p 8000:8000 amazon/dynamodb-local
docker run -d -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb

## Build & Start API
docker rmi --force aspnetapp
dotnet publish -c Release -o out
docker build -t aspnetapp .
docker run -p 8080:80 aspnetapp

## Query
aws dynamodb query --table-name PortfolioEvents \
				   --key-condition-expression "ContractId = :ContractId" 
				   --expression-attribute-values '{ ":ContractId": { "S" : "A100"}}' 
				   --endpoint-url http://localhost:8000

aws dynamodb query --table-name Portfolio \
				   --key-condition-expression "ContractId = :ContractId" 
				   --expression-attribute-values '{ ":ContractId": { "S" : "A100"}}' 
				   --endpoint-url http://localhost:8000
