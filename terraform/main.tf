provider "google" {
  credentials = file("account.json")
  project     = "my-project-id"
  region      = "us-central1"
}

resource "google_project" "project" {
  name       = "quarentime-terraform-test"
  project_id = "quarentime-terraform-test"
  org_id     = var.org_id
}


