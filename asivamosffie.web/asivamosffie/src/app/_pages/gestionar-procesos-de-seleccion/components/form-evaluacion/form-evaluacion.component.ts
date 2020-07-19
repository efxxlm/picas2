import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-evaluacion',
  templateUrl: './form-evaluacion.component.html',
  styleUrls: ['./form-evaluacion.component.scss']
})
export class FormEvaluacionComponent {
  addressForm = this.fb.group({
    descricion: [null, Validators.required],
    url: [null, Validators.required]
  });

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(private fb: FormBuilder) {}

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
