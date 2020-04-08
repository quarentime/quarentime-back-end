
/* Service account to be used by apis and permissions */
resource "google_service_account" "api_service_account" {
  account_id   = "api_service_account"
  display_name = "Api service account"
  project      = google_project.project.project_id
}

resource "google_project_iam_binding" "api_service_account_enqueuer" {
  project = google_project.project.project_id
  role    = "roles/cloudtasks.enqueuer"

  members = [
    "serviceAccount:${google_service_account.api_service_account.email}",
  ]
}

resource "google_project_iam_binding" "api_service_account_data_store_user" {
  project = google_project.project.project_id
  role    = "roles/datastore.user"

  members = [
    "serviceAccount:${google_service_account.api_service_account.email}",
  ]
}

resource "google_project_iam_binding" "api_service_account_logs_writer" {
  project = google_project.project.project_id
  role    = "roles/logging.logWriter"

  members = [
    "serviceAccount:${google_service_account.api_service_account.email}",
  ]
}
