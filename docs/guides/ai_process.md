# Product Strategy

## Opportunity

Customers question the authenticity of job photos. There's a need for verifiable proof (time, location, integrity) to reduce disputes and build trust.

## North Star

Deliver indisputable visual proof of job completion via timestamped, geo-tagged, AI-labeled photos.

## OKRs

### Objective 1: Improve customer trust in job documentation

-   KR1: 90% of customers report high confidence in photo authenticity
-   KR2: 80% reduction in disputes related to job photo evidence

### Objective 2: Reduce operational overhead and inefficiencies

-   KR1: 50% fewer follow-up requests for job clarification
-   KR2: 25% cost reduction in job verification efforts

## Initiatives

-   Build Inspector mobile interface to capture trusted job photos with metadata
-   Develop Customer portal to view trusted photo documentation
-   Integrate AI labeling to categorize and describe photos automatically
-   Implement secure, verifiable metadata storage and retrieval plan

# Product Plan

## Milestones

1.  Discovery Complete – Validate user needs, workflows, and technical feasibility
2.  Prototype Built – Basic Inspector and Customer flows demo-ready
3.  MVP Released – Core features launched to pilot users
4.  Feedback Loop – Gather usage data and refine product
5.  Scale & Harden – Security, performance, and expansion features added

## Deliverables

-   Journey maps for Inspectors and Customers
-   Functional prototypes (mobile and web)
-   Verified photo upload module with timestamp, GPS, AI tagging
-   Customer dashboard with photo timeline and metadata
-   Metadata integrity service (e.g., tamper-evident storage)
-   Admin tools for job management and support

## Dependencies

-   Access to real job workflows for Inspectors and Customers
-   GPS and camera access on Inspector devices
-   AI service for photo categorization and labeling
-   Secure storage for metadata and photos
-   Authentication and user role management

# Feature Requirements

Each feature includes user stories (e.g., "As an Inspector..."), acceptance criteria (e.g., "photo includes GPS accurate to 10m"), and test cases (e.g., "login fails with invalid credentials").

## Inspector Login & Job Selection

-   Secure login
-   View and select assigned jobs

### Stories

ST 1.1: As an Inspector, I want to log in securely so that I can access my assigned jobs.

ST 1.2: As an Inspector, I want to view a list of my jobs so I can choose one to start working on.

### Acceptance Criteria

AC 1.1.1: Login requires valid email and password

AC 1.1.2: Invalid credentials return an error message

AC 1.2.1: Job list loads upon successful login

AC 1.2.2: Job list includes job name, address, and status

AC 1.2.3: Inspector can select a job to begin photo documentation

### Test Cases

TC 1.1.1: Login succeeds with valid credentials

TC 1.1.2: Login fails with invalid credentials

TC 1.2.1: Job list appears after login

TC 1.2.2: Job details display correctly

TC 1.2.3: Job selection navigates to the photo capture screen

## Photo Capture with Metadata

-   Capture photo in-app
-   Auto-tag with timestamp and GPS
-   Auto-generate AI category and description
-   Store photo and metadata securely

### Stories

ST 2.1: As an Inspector, I want to capture a photo from within the app so that it can be verified and attached to the job.

ST 2.2: As an Inspector, I want the photo to automatically record timestamp and GPS so that I don’t have to enter them manually.

ST 2.3: As an Inspector, I want the system to auto-generate a category and description using AI to reduce manual effort.

### Acceptance Criteria

AC 2.1.1: In-app camera opens when a job is selected

AC 2.1.2: Captured photo is previewed before submission

AC 2.2.1: Photo includes timestamp and GPS coordinates

AC 2.2.2: Location data matches current device position

AC 2.3.1: AI-generated category and description appear after photo capture

AC 2.3.2: Inspector can approve or edit the AI-generated description

### Test Cases

TC 2.1.1: Inspector can capture photo from in-app camera

TC 2.1.2: Inspector can edit or approve AI-generated text

TC 2.1.3: Photo and metadata are submitted and stored securely

TC 2.2.1: Photo includes timestamp accurate to the second

TC 2.2.2: Photo includes GPS coordinates accurate to 10 meters

TC 2.3.1: AI generates photo category and description

## Customer Job Viewer

-   View list of jobs and statuses
-   View photo timeline with metadata
-   Filter by job, date, or category

### Stories

ST 3.1: As a Customer, I want to view a list of my jobs so I can track their progress.

ST 3.2: As a Customer, I want to view photos for a selected job to verify its completion.

ST 3.3: As a Customer, I want to see timestamps, GPS info, and AI descriptions for each photo so I can trust its authenticity.

### Acceptance Criteria

AC 3.1.1: Customer dashboard displays all jobs assigned to their account

AC 3.1.2: Each job displays name, status, and last update

AC 3.2.1: Clicking a job reveals a photo timeline

AC 3.3.1: Each photo displays timestamp, GPS, and AI-generated description

AC 3.3.2: Photos are shown in chronological order with metadata clearly visible

### Test Cases

TC 3.1.1: Job list appears on customer login

TC 3.1.2: Job status and updates are visible

TC 3.2.1: Clicking a job opens the photo timeline

TC 3.3.1: Each photo includes timestamp and GPS metadata

TC 3.3.2: AI descriptions are shown alongside photos

TC 3.3.3: Photos are sorted by date/time

## Metadata Integrity Service

-   Ensure metadata is tamper-evident
-   Log photo verification events

### Stories

ST 4.1: As the system, I want to ensure metadata cannot be altered after capture so that customers can trust the evidence.

ST 4.2: As an Admin, I want to view logs of metadata validation events to monitor integrity and detect tampering.

### Acceptance Criteria

AC 4.1.1: Photo and metadata are cryptographically hashed upon capture

AC 4.1.2: Any modification attempt invalidates the hash

AC 4.1.3: System stores hash and original data in secure storage

AC 4.2.1: Admin panel shows a validation log with timestamps and status

AC 4.2.2: Discrepancies are flagged for review

### Test Cases

TC 4.1.1: Captured data is hashed on submission

TC 4.1.2: Modified data fails hash validation

TC 4.1.3: Hashes are stored and verifiable

TC 4.2.1: Admin can view validation logs

TC 4.2.2: Tampered records are flagged correctly

## Admin Portal (Basic)

-   Manage users and jobs
-   View logs and dispute flagsStories, Acceptance Criteria & Test Cases

### Stories

ST 5.1: As an Admin, I want to manage users and assign jobs so I can control access and workflows.

ST 5.2: As an Admin, I want to view system logs and flagged items to support operational monitoring.

### Acceptance Criteria

AC 5.1.1: Admins can create, update, and deactivate user accounts

AC 5.1.2: Admins can assign jobs to Inspectors

AC 5.2.1: Admin dashboard displays logs of metadata validations and job submissions

AC 5.2.2: Flagged jobs or photos are clearly marked with status

### Test Cases

TC 5.1.1: Admin can create and manage user accounts

TC 5.1.2: Admin can assign and reassign jobs

TC 5.2.1: System logs display validation and submission history

TC 5.2.2: Flagged records are visible with appropriate labels

# Feature Roadmap

## 🌱 Phase 1: MVP Foundation (Trust First)

**Goal:** Establish photo authenticity and basic Inspector/Customer flow.

### ✅ Must-Have

-   Inspector Login & Job Selection (Feature 1)
-   Photo Capture with Metadata (Feature 2)
-   Customer Job Viewer (Feature 3)
-   Metadata Integrity Service (Feature 4)

🗓 Estimated Duration: 6–8 weeks (target pilot-ready version)

## 🌿 Phase 2: Operational Tools (Efficiency Layer)

**Goal:** Support internal management and monitoring

### 🚧 Should-Have

-   Admin Portal (Basic) (Feature 5)
-   Advanced AI customization (training, labeling feedback)
-   Inspector photo review and correction workflows

🗓 Estimated Duration: 4–6 weeks post-MVP

## 🌳 Phase 3: Scale & Optimize

**Goal:** Enhance value, reduce friction, and build trust at scale

### 🌟 Nice-to-Have

-   Customer-side alerts/notifications (photo uploaded, job complete)
-   Automated dispute handling via audit logs
-   Analytics dashboard (job quality, dispute trends)

🗓 Parallel or post-feedback cycles
