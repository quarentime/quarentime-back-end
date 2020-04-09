resource "google_cloud_tasks_queue" "queue_push" {
  project  = data.google_project.project.project_id
  name     = "push-notifications"
  location = "europe-west1"

  rate_limits {
    max_dispatches_per_second = 500
    max_concurrent_dispatches = 1000
  }
}

resource "google_cloud_tasks_queue" "queue_sms" {
  project  = data.google_project.project.project_id
  name     = "sms-notifications"
  location = "europe-west1"

  rate_limits {
    max_dispatches_per_second = 500
    max_concurrent_dispatches = 1
  }
}
