import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-tabla-cronograma',
  templateUrl: './tabla-cronograma.component.html',
  styleUrls: ['./tabla-cronograma.component.scss']
})
export class TablaCronogramaComponent implements OnInit {

  addressForm = this.fb.array([
      this.fb.group({
        descripcion: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(500)
        ])],
        fecha: [null, Validators.required]
      })
    ]);

  editorStyle = {
    height: '100px',
    width: '600px'
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

  constructor(
    private fb: FormBuilder
  ) {
    this.maxDate = new Date();
  }

  ngOnInit(): void {
  }

  agregaFuente() {
    this.addressForm.push(this.crearActividad());
  }

  crearActividad(): FormGroup {
    return this.fb.group({
      descripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(500)
      ])],
      fecha: [null, Validators.required]
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {
    console.log('--');
  }

}
