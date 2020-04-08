/* The default compute account need access to cloud run */
resource "google_project_iam_binding" "compute_service_account_run_invoker" {
  project = google_project.project.project_id
  role    = "roles/run.invoker"

  members = [
    "serviceAccount:${google_project.project.number}-compute@developer.gserviceaccount.com"
  ]
}