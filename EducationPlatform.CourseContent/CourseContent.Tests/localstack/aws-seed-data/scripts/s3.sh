#!/bin/sh

awslocal s3 mb s3://my-bucket --endpoint http://localhost:4566 --profile localstack
