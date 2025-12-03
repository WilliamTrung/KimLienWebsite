#!/bin/bash

DOMAIN="ctytnhhkimlien.vn"
CERT_DIR="/root/certs"

mkdir -p $CERT_DIR

openssl req -x509 -newkey rsa:2048 -nodes \
  -keyout "$CERT_DIR/ssl.key" \
  -out "$CERT_DIR/ssl.crt" \
  -days 730 \
  -subj "/CN=$DOMAIN"

chmod 600 $CERT_DIR/ssl.*

echo "Self-signed certificate generated:"
ls -l $CERT_DIR
