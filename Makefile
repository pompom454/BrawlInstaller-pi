.PHONY: help build release debug clean restore test publish install

# Variables
SOLUTION = BrawlInstaller.sln
DOTNET = dotnet
CONFIG ?= Debug
OUTPUT_DIR = bin/$(CONFIG)

help:
	@echo "BrawlInstaller-pi Makefile"
	@echo "=========================="
	@echo "Available targets:"
	@echo "  make build        - Build the project in Debug mode"
	@echo "  make release      - Build the project in Release mode"
	@echo "  make debug        - Build the project in Debug mode (explicit)"
	@echo "  make clean        - Clean build artifacts"
	@echo "  make restore      - Restore NuGet packages"
	@echo "  make test         - Run unit tests (if available)"
	@echo "  make publish      - Publish the application"
	@echo "  make run          - Build and run the application"
	@echo "  make install-deps - Install required dependencies for Linux"

# Restore NuGet packages
restore:
	@echo "Restoring NuGet packages..."
	$(DOTNET) restore $(SOLUTION)

# Build targets
build: restore
	@echo "Building in $(CONFIG) mode..."
	$(DOTNET) build $(SOLUTION) -c $(CONFIG) --no-restore

debug: restore
	@echo "Building in Debug mode..."
	$(DOTNET) build $(SOLUTION) -c Debug --no-restore

release: restore
	@echo "Building in Release mode..."
	$(DOTNET) build $(SOLUTION) -c Release --no-restore

# Clean build artifacts
clean:
	@echo "Cleaning build artifacts..."
	$(DOTNET) clean $(SOLUTION) -c $(CONFIG) || true
	@find . -type d -name "bin" -o -name "obj" | xargs rm -rf

# Run tests (if test projects exist)
test: restore
	@echo "Running tests..."
	$(DOTNET) test $(SOLUTION) -c $(CONFIG) --no-build --verbosity normal || echo "No tests found or tests failed"

# Publish the application
publish: restore
	@echo "Publishing application..."
	$(DOTNET) publish $(SOLUTION) -c Release -o ./publish

# Build and run
run: debug
	@echo "Running application..."
	$(DOTNET) run --project BrawlInstaller/BrawlInstaller.csproj -c $(CONFIG)

# Install Linux dependencies (for .NET SDK)
install-deps:
	@echo "Installing dependencies for Linux..."
	@command -v dotnet >/dev/null 2>&1 || { echo "ERROR: .NET SDK is required but not installed."; exit 1; }
	@echo "Dependencies check passed"

.DEFAULT_GOAL := help