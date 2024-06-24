#!/bin/sh

aws configure --profile localstack <<EOF
dummy-access-key
dummy-secret-key
us-east-1
json
EOF

/scripts/s3.sh

exit 0
