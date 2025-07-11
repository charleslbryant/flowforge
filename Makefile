install:
	bash ./scripts/forge install

demo:
	bash ./scripts/run-demo.sh

nuke:
	bash ./scripts/nuke.sh

# Testing commands
test:
	@echo "🧪 Running FlowForge test suite..."
	@bats tests/

test-verbose:
	@echo "🧪 Running FlowForge tests with verbose output..."
	@bats --verbose-run tests/

test-forge:
	@echo "🧪 Testing scripts/forge..."
	@bats tests/test_forge.bats

test-api:
	@echo "🧪 Testing scripts/n8n-api.sh..."
	@bats tests/test_n8n_api.bats

test-workflow:
	@echo "🧪 Testing workflow creation..."
	@bats tests/test_workflow_creation.bats

test-watch:
	@echo "🔄 Running tests in watch mode..."
	@while inotifywait -e modify tests/ scripts/ 2>/dev/null; do \
		echo "📝 Files changed, running tests..."; \
		make test; \
	done

test-install:
	@echo "🔧 Installing test dependencies..."
	@command -v bats >/dev/null 2>&1 || { \
		echo "Installing Bats..."; \
		npm install -g bats; \
	}
	@command -v inotifywait >/dev/null 2>&1 || { \
		echo "Installing inotify-tools for watch mode..."; \
		sudo apt-get update && sudo apt-get install -y inotify-tools; \
	}

test-clean:
	@echo "🧹 Cleaning test artifacts..."
	@rm -rf test_temp_* mock_temp_* *.test.log .bats/ test-reports/

test-report:
	@echo "🧪 Running FlowForge test suite with report..."
	@mkdir -p test-reports
	@bats --formatter tap tests/ > test-reports/results.tap 2>&1 || true
	@bats tests/ > test-reports/results.txt 2>&1 || true
	@echo "📊 Test reports generated in test-reports/"
	@echo "   - test-reports/results.tap (TAP format)"
	@echo "   - test-reports/results.txt (Human readable)"

help:
	@echo "FlowForge Commands:"
	@echo "  install      - Install n8n and setup FlowForge"
	@echo "  demo         - Run FlowForge demonstration"
	@echo "  nuke         - Remove n8n and reset environment"
	@echo ""
	@echo "Testing Commands:"
	@echo "  test         - Run all tests"
	@echo "  test-verbose - Run tests with verbose output"
	@echo "  test-report  - Generate test reports"
	@echo "  test-forge   - Test scripts/forge only"
	@echo "  test-api     - Test scripts/n8n-api.sh only"
	@echo "  test-workflow - Test workflow creation only"
	@echo "  test-watch   - Run tests in watch mode"
	@echo "  test-install - Install test dependencies"
	@echo "  test-clean   - Clean test artifacts"
