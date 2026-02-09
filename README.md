# Disabled Users License Report

This tool detects **disabled Microsoft Entra ID users who are still consuming licenses**, including licenses assigned through **groups**, using Microsoft Graph and .NET.

It helps administrators identify users who may still be incurring licensing costs after being disabled.

## Features
- Lists disabled users  
- Shows assigned licenses  
- Identifies group-based licenses  
- Displays licensing group names  

## Prerequisites

### Configure App Registration
1. Go to **Microsoft Entra Admin Center**
2. Navigate to:
   Applications → App registrations
3. Create a new app or open an existing one
4. Go to **API permissions**
5. Add:
   - User.Read.All (Application)
   - Directory.Read.All (Application)
6. Click **Grant Admin Consent**

### Create Client Secret
1. Go to **Certificates & secrets**
2. Click **New client secret**
3. Copy the value

Once Above Steps Completed

### Create Project
1. Open **Visual Studio**
2. Click **Create a new project**
3. Select **Console App (.NET)**
4. Click **Next**
5. Enter project name (CheckDisabledUsersLicenses)
6. Click **Create**

### Install NuGet Packages
1. Right-click the project  
2. Select **Manage NuGet Packages**
3. Install:

- Microsoft.Graph  
- Azure.Identity  

### Update Code
Update these values in `Program.cs`:

- tenantId  
- clientId  
- clientSecret  


### Step 6 – Run the Application

Run from Visual Studio:
