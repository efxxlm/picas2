import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-registrar-seguimiento-cronograma',
  templateUrl: './registrar-seguimiento-cronograma.component.html',
  styleUrls: ['./registrar-seguimiento-cronograma.component.scss']
})
export class RegistrarSeguimientoCronogramaComponent {
  addressForm = this.fb.array([
    this.fb.group({
      fechaMonitoreo: [null, Validators.required],
      estadoActividad: [null, Validators.required],
      observación: [null, Validators.required]
    })
  ]);

  hasUnitNumber = false;

  states = [
    { name: 'Alabama', value: 'AL' },
    { name: 'Alaska', value: 'AK' },
    { name: 'American Samoa', value: 'AS' },
    { name: 'Arizona', value: 'AZ' },
    { name: 'Arkansas', value: 'AR' },
    { name: 'California', value: 'CA' },
    { name: 'Colorado', value: 'CO' },
    { name: 'Connecticut', value: 'CT' },
    { name: 'Delaware', value: 'DE' },
    { name: 'District Of Columbia', value: 'DC' },
    { name: 'Federated States Of Micronesia', value: 'FM' }
  ];

  editorStyle = {
    height: '100px',
    width: '300px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  maxDate: Date;

  constructor(private fb: FormBuilder) {
    this.maxDate = new Date();
  }


  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  onSubmit() {
    console.log(this.addressForm);
  }
}
