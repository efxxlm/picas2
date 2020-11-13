import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router'
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-seguimiento',
  templateUrl: './form-registrar-seguimiento.component.html',
  styleUrls: ['./form-registrar-seguimiento.component.scss']
})
export class FormRegistrarSeguimientoComponent {

  seguimientoId: string;

  addressForm = this.fb.group({
    fechaSeguimiento: [null, Validators.required],
    disponibilidadPersonal: [null, Validators.required],
    disponibilidadPersonalObservaciones: [null, Validators.required],
    disponibilidadMaterial: [null, Validators.required],
    disponibilidadMaterialObservaciones: [null, Validators.required],
    disponibilidadEquipo: [null, Validators.required],
    disponibilidadEquipoObservaciones: [null, Validators.required],
    Productividad: [null, Validators.required],
    ProductividadObservaciones: [null, Validators.required]
  });

  minDate: Date;
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

  arraySelect = [
    {name: 'Alabama', value: 'AL'},
    {name: 'Alaska', value: 'AK'}
  ];

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private dialog: MatDialog
    ) {
    this.minDate = new Date();
  }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId);
      
    })
  }

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog( '', '<b>La información ha sido guardada exitosamente.</b>' );
  }
}
