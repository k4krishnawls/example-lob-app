module.exports = function (wallaby) {
  return {
    files: [
      "src/**/*.[jt]sx?",
      "!src/**/*.spec.[jt]sx?"
    ],
    tests: [
      "src/**/*.spec.[jt]sx?"
    ],

    compilers: {
      'src/**/*.ts': wallaby.compilers.typeScript({
        isolatedModules: true
      })
    },
    env: {
      type: 'node',
      runner: 'node'
    },
    testFramework: 'jest',
    debug: true,
    setup: function (wallaby) {
      var jestConfig = require(wallaby.localProjectDir + '/jest.config.js');
      jestConfig.moduleDirectories = [
        'node_modules'
      ];
      wallaby.testFramework.configure(jestConfig);
    },
    trace: true
  };
};
