import { GeneratorConfig } from 'ng-openapi';

const config: GeneratorConfig = {
  input: 'http://localhost:5041/Swagger/v1/swagger.json',
  output: './src/app/api/http',
  options: {
    dateType: 'Date',
    enumStyle: 'enum',
    generateEnumBasedOnDescription: true,
    generateServices: true,
    responseTypeMapping: {
      'application/pdf': 'blob',
      'application/zip': 'blob',
      'text/csv': 'text'
    }
  }
};

export default config;