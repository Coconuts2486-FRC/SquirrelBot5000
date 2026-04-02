#!/bin/bash

# Prompt for commit message
read -p "Enter commit message: " commit_message

# Get current branch name
current_branch=$(git branch --show-current)

# Check if branch was found
if [ -z "$current_branch" ]; then
  echo "Error: Could not determine current branch."
  exit 1
fi

# Add, commit, and push
git add --all
git commit -m "$commit_message"

# Push to current branch (set upstream if needed)
git push -u origin "$current_branch"

# Show status
git status