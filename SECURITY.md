# Security Policy

## Supported Versions

We release patches for security vulnerabilities. Which versions are eligible for receiving such patches depends on the CVSS v3.0 Rating:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reporting a Vulnerability

We take the security of our online course platform seriously. If you have discovered a security vulnerability, we appreciate your help in disclosing it to us in a responsible manner.

### How to Report a Security Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report them via one of the following methods:

1. **GitHub Security Advisories** (Preferred)
   - Go to the [Security tab](https://github.com/aeff60/online-course-platform/security) of this repository
   - Click "Report a vulnerability"
   - Fill out the form with details about the vulnerability

2. **Email**
   - Send an email to: security@example.com (replace with your actual security contact email)
   - Include the word "SECURITY" in the subject line

### What to Include in Your Report

Please include the following information in your report:

- Type of vulnerability (e.g., SQL injection, XSS, authentication bypass, etc.)
- Full paths of source file(s) related to the vulnerability
- The location of the affected source code (tag/branch/commit or direct URL)
- Any special configuration required to reproduce the issue
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the issue, including how an attacker might exploit it

### What to Expect

- **Acknowledgment**: We will acknowledge receipt of your vulnerability report within 48 hours
- **Communication**: We will keep you informed about the progress of fixing the vulnerability
- **Timeline**: We aim to patch critical vulnerabilities within 30 days
- **Credit**: We will give you credit for the discovery in our security advisory (unless you prefer to remain anonymous)

## Security Best Practices for Users

If you're using or deploying this online course platform, please follow these security best practices:

1. **Keep Updated**: Always use the latest stable version
2. **Secure Configuration**: 
   - Change all default passwords
   - Use strong, unique passwords for all accounts
   - Enable two-factor authentication where available
3. **Environment Variables**: Never commit sensitive data (API keys, passwords, secrets) to the repository
4. **HTTPS**: Always use HTTPS in production environments
5. **Database Security**: 
   - Use parameterized queries to prevent SQL injection
   - Regularly backup your database
   - Restrict database access to necessary services only
6. **Input Validation**: Validate and sanitize all user inputs
7. **Access Control**: Implement proper role-based access control (RBAC)
8. **Monitoring**: Enable logging and monitoring to detect suspicious activities

## Security Features

Our platform includes the following security features:

- Authentication and authorization mechanisms
- Password hashing using industry-standard algorithms
- Protection against common web vulnerabilities (XSS, CSRF, SQL Injection)
- Session management and timeout mechanisms
- Secure file upload handling
- Rate limiting to prevent abuse

## Disclosure Policy

- We will work with you to understand and resolve the issue
- We will not take legal action against security researchers who:
  - Act in good faith
  - Report vulnerabilities responsibly
  - Do not access or modify user data beyond what is necessary to demonstrate the vulnerability
  - Do not publicly disclose the vulnerability until we have had a reasonable time to address it

## Updates to This Policy

We may update this security policy from time to time. We will notify users of any material changes by updating the date at the bottom of this policy.

---

**Last Updated**: 2025-12-17