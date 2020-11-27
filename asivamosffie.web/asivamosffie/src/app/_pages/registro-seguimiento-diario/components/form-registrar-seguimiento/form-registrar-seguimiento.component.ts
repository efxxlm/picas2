import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-seguimiento',
  templateUrl: './form-registrar-seguimiento.component.html',
  styleUrls: ['./form-registrar-seguimiento.component.scss']
})
export class FormRegistrarSeguimientoComponent implements OnInit {

  seguimientoId: string;

  addressForm = this.fb.group({
    fechaSeguimiento: [null, Validators.required],
    disponibilidadPersonal: [null, Validators.required],
    cantidadPersonalOperativoProgramado: [null, Validators.compose([
      Validators.required, Validators.maxLength(3), Validators.max(999)])
    ],
    cantidadPersonalOperativoTrabajando: [null, Validators.compose([
      Validators.required, Validators.maxLength(3), Validators.max(999)])
    ],
    retrasoPersonal: [null, Validators.required],
    horasRetrasoPersonal:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadPersonalObservaciones: [null, Validators.required],
    disponibilidadMaterial: [null, Validators.required],
    causaMaterial: [null, Validators.required],
    retrasoMaterial: [null, Validators.required],
    horasRetrasoMaterial:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadMaterialObservaciones: [null, Validators.required],
    disponibilidadEquipo: [null, Validators.required],
    causaEquipo: [null, Validators.required],
    retrasoEquipo: [null, Validators.required],
    horasRetrasoEquipo:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    disponibilidadEquipoObservaciones: [null, Validators.required],
    Productividad: [null, Validators.required],
    causaProductividad: [null, Validators.required],
    retrasoProductividad: [null, Validators.required],
    horasRetrasoProductividad:  [null, Validators.compose([
      Validators.required, Validators.maxLength(1), Validators.min(1), Validators.max(9)])
    ],
    ProductividadObservaciones: [null, Validators.required]
  });

  minDate: Date;
  editorStyle = {
    // height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  personalArray = [
    { name: 'Suficiente', value: 'suficiente' },
    { name: 'Insuficiente', value: 'insuficiente' }
  ];
  materialArray = [
    { name: 'Óptima', value: 'optima' },
    { name: 'Media', value: 'media' },
    { name: 'Baja', value: 'baja' }
  ];
  equipolArray = [
    { name: 'Total', value: 'total' },
    { name: 'Parcial', value: 'parcial' },
    { name: 'Baja', value: 'baja' }
  ];
  productividadArray = [
    { name: 'Alta', value: 'Alta' },
    { name: 'Media', value: 'media' },
    { name: 'Baja', value: 'baja' }
  ];
  causaBajaDisponibilidadMaterial = [
    { name: 'No se realizó el pedido del material', value: '1' },
    { name: 'incumplimiento proveedor', value: '2' },
    { name: 'imposibilidad de entrega de material por motivos de fuerza mayor', value: '3' }
  ];
  causaBajaDisponibilidadEquipo = [
    { name: 'En mantenimiento', value: '1' },
    { name: 'No contratado', value: '2' },
    { name: 'incumplimiento de proveedor', value: '3' },
    { name: 'imposibilidad de entrega de equipos por motivos de fuerza mayor', value: '4' }
  ];
  causaBajaDisponibilidadProductividad = [
    { name: 'Condiciones climáticas', value: '1' },
    { name: 'Paros o inconvenientes con la comunidad', value: '2' },
    { name: 'Accidente laboral', value: '3' },
    { name: 'Orden público', value: '4' },
    { name: 'Otros', value: '5' },
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

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId);
    });
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.addressForm.markAllAsTouched();
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
