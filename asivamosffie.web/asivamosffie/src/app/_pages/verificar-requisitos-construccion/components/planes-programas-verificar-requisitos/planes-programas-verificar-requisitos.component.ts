import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-planes-programas-verificar-requisitos',
  templateUrl: './planes-programas-verificar-requisitos.component.html',
  styleUrls: ['./planes-programas-verificar-requisitos.component.scss']
})
export class PlanesProgramasVerificarRequisitosComponent implements OnInit {

  dataPlanesProgramas: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'planesProgramas',
    'recibioRequisito',
    'fechaRadicado',
    'fechaAprobacion',
    'requiereObservacion',
    'observaciones'
  ];
  booleanosRequisitos: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  require: any;
  booleanosObservacion: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  urlSoporte: string;
  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
  });

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor ( private dialog: MatDialog,private fb: FormBuilder ) {
    this.getDataPlanesProgramas();
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataPlanesProgramas );
  }


  guardar () {
    console.log( this.dataPlanesProgramas );
    console.log( this.urlSoporte );
  }

  getDataPlanesProgramas () {
    this.dataPlanesProgramas.push(
      {
        nombrePlanesProgramas: 'Licencia vigente',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 7
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 12
      }
    );
  };
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  onSubmit(){
    this.openDialog( 'La información ha sido guardada exitosamente.', '' );
  }
}
