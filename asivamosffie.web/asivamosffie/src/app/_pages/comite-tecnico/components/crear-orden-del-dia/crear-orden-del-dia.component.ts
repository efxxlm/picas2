import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SolicitudesContractuales, Sesion, SesionComiteTema, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  solicitudesContractuales: SolicitudesContractuales[] = [];
  fechaSesionString: string;
  fechaSesion: Date;
  idSesion: number = 0;
  estadosComite = EstadosComite;
  objetoSesion: Sesion = {
    estadoComiteCodigo: this.estadosComite.sinConvocatoria
  };

  addressForm = this.fb.group({
      tema: this.fb.array([]),
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
              private router: Router,

             ) 
    {

    }


  ngOnInit(): void {
    this.activatedRoute.params.subscribe( a => {
      let fecha = Date.parse(a.fecha);
       this.fechaSesion = new Date(fecha);
       this.fechaSesionString = `${ this.fechaSesion.getFullYear() }/${ this.fechaSesion.getMonth() + 1 }/${ this.fechaSesion.getDate() }` 

       this.idSesion = a.id;

       if (this.idSesion > 0){
        this.editMode();
       }else{

        this.agregaTema();

        this.techicalCommitteeSessionService.getListSolicitudesContractuales( this.fechaSesionString )
        .subscribe( response => {

          this.solicitudesContractuales = response;
          

          setTimeout(() => {
            
            let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
            btnTablaSolicitudes.click();
            console.log( this.solicitudesContractuales, this.objetoSesion );

          }, 1000);

        });
      }

    })
  }

  editMode(){

    forkJoin([ 
      this.techicalCommitteeSessionService.getListSesionComiteTemaByIdSesion( this.idSesion ),
      this.techicalCommitteeSessionService.getSesionBySesionId( this.idSesion ),

     ]).subscribe( response => {

      this.objetoSesion = response[1];

        let temas = this.addressForm.get('tema') as FormArray;

        temas.clear();

        response[0].forEach( te => {
          let grupoTema = this.crearTema();

          grupoTema.get('tema').setValue( te.tema );
          grupoTema.get('responsable').setValue( te.responsableCodigo );
          grupoTema.get('tiempoIntervencion').setValue( te.tiempoIntervencion );
          grupoTema.get('url').setValue( te.rutaSoporte );
          grupoTema.get('sesionTemaId').setValue( te.sesionTemaId );
          

          temas.push( grupoTema );

        })

        console.log( response );
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
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        //Validators.required,
        //Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {

    console.log(this.addressForm);
    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');

    }else{
      let sesion: Sesion = {
        sesionId: this.idSesion,
        fechaOrdenDia: this.fechaSesion,
        sesionComiteTema: []
      }
  
      this.tema.controls.forEach( control => {
        let sesionComiteTema: SesionComiteTema = {
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,
          sesionTemaId: control.get('sesionTemaId').value,
          sesionId: this.idSesion,

        }
  
        sesion.sesionComiteTema.push( sesionComiteTema );
      }) 
  
  
      this.techicalCommitteeSessionService.saveEditSesionComiteTema( sesion ).subscribe( respuesta => {
          this.openDialog( 'Sesion Comite', respuesta.message )
          if ( respuesta.code == "200" )
            this.router.navigate(['/comiteTecnico']);
      });
    }
  }
}
