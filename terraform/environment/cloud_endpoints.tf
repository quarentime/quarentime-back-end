/* The default compute account need access to cloud run */
resource "google_project_iam_binding" "compute_service_account_run_invoker" {
  project = data.google_project.project.project_id
  role    = "roles/run.invoker"

  members = [
    "serviceAccount:${data.google_project.project.number}-compute@developer.gserviceaccount.com"
  ]
}

resource "google_cloud_run_service" "quarentime_gateway" {
  project  = data.google_project.project.project_id
  name     = "quarentime-gateway"
  location = "europe-west1"

  template {
    spec {
      containers {
        image = "gcr.io/endpoints-release/endpoints-runtime-serverless:2"
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

data "google_iam_policy" "noauth" {
  binding {
    role = "roles/run.invoker"
    members = [
      "allUsers",
    ]
  }
}

resource "google_cloud_run_service_iam_policy" "noauth" {
  location = google_cloud_run_service.quarentime_gateway.location
  project  = google_cloud_run_service.quarentime_gateway.project
  service  = google_cloud_run_service.quarentime_gateway.name

  policy_data = data.google_iam_policy.noauth.policy_data
}
