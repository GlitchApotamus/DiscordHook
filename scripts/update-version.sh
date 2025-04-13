#!usr/bin/env bash

set -e
cd "$(dirname "$0")/.." || exit 1


# Find the .csproj file in the current directory
csproj=$(find . -maxdepth 1 -name "*.csproj" | head -n 1)

if [[ ! -f "$csproj" ]]; then
  echo "[ERROR] No .csproj file found in current directory"
  exit 1
fi

echo "[INFO] Using project file: $csproj"

# Extract current versions
pversion=$(grep -oPm1 '(?<=<PVersion>)[^<]+' "$csproj")
nversion=$(grep -oPm1 '(?<=<NVersion>)[^<]+' "$csproj")

if [[ -z "$pversion" || -z "$nversion" ]]; then
  echo "[ERROR] Could not parse PVersion or NVersion"
  exit 1
fi

echo "[INFO] Current PVersion: $pversion"
echo "[INFO] Current NVersion: $nversion"

# Split NVersion into parts
IFS='.' read -r major minor patch <<< "$nversion"

if [[ -z "$major" || -z "$minor" || -z "$patch" ]]; then
  echo "[ERROR] NVersion '$nversion' is not in the expected format (X.Y.Z)"
  exit 1
fi

# Bump patch version
new_version="$major.$minor.$((patch + 1))"

echo "[INFO] Bumping version: $nversion → $new_version"

# Create backup and update the file in-place
cp "$csproj" "$csproj.bak"

sed -i "s|<PVersion>.*</PVersion>|<PVersion>$nversion</PVersion>|" "$csproj"
sed -i "s|<NVersion>.*</NVersion>|<NVersion>$new_version</NVersion>|" "$csproj"

echo "[SUCCESS] Updated PVersion to $nversion and NVersion to $new_version"
rm "$csproj.bak"

