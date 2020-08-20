import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SolicitudesContractuales, Sesion, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  solicitudesContractuales: SolicitudesContractuales[] = [];
  fechaSesionString: string;
  fechaSesion: Date;

  addressForm = this.fb.group({
    tema: this.fb.array([
      this.fb.group({
        tema: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ],
        responsable: [null, Validators.required],
        tiempoIntervencion: [null, Validators.compose([
          Validators.required, Validators.minLength(1), Validators.maxLength(3)])
        ],
        url: [null, [
          Validators.required,
          Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
        ]]
      })
    ])
  });


  responsablesArray = [
    {name: 'reponsable 1', value: '1'},
    {name: 'reponsable 2', value: '2'},
    {name: 'reponsable 3', value: '3'}
  ];

  constructor(
              private fb: FormBuilder,
              public dialog: MatDialog,
              private activatedRoute: ActivatedRoute,
              private techicalCommitteeSessionService: TechnicalCommitteSessionService,

             ) 
    {

    }


  ngOnInit(): void {
    this.activatedRoute.params.subscribe( a => {
      let fecha = Date.parse(a.fecha);
       this.fechaSesion = new Date(fecha);
       this.fechaSesionString = `${ this.fechaSesion.getFullYear() }/${ this.fechaSesion.getMonth() + 1 }/${ this.fechaSesion.getDate() }` 

      this.techicalCommitteeSessionService.getListSolicitudesContractuales( this.fechaSesionString )
      .subscribe( response => {

        this.solicitudesContractuales = response;

        setTimeout(() => {
          
          let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
          btnTablaSolicitudes.click();
          console.log( this.solicitudesContractuales );

        }, 1000);

      });

    })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  crearTema() {
    return this.fb.group({
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        Validators.required,
        Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {

    let sesion: Sesion = {
      fechaOrdenDia: this.fechaSesion,
      sesionComiteTema: []
    }

    this.tema.controls.forEach( control => {
      let sesionComiteTema: SesionComiteTema = {
        tema: control.get('tema').value,
        ResponsableCodigo: control.get('responsable').value,
        tiempoIntervencion: control.get('tiempoIntervencion').value,
        rutaSoporte: control.get('url').value,

      }

      sesion.sesionComiteTema.push( sesionComiteTema );
    }) 


    this.techicalCommitteeSessionService.saveEditSesionComiteTema( sesion ).subscribe( response => {
      console.log(response);
    });

    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');
    }
  }
}
