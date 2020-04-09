provider "google" {
  credentials = file("/tmp/quarentime-prod-terraform.json")
  project     = "my-project-id"
  region      = "us-central1"
}

module "environment" {
  source = "../environment"
  project_id = "quarentime-prod"
}
