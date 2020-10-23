import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ComiteTecnico, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { Router } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';

@Component({
  selector: 'app-form-proposiciones-varios',
  templateUrl: './form-proposiciones-varios.component.html',
  styleUrls: ['./form-proposiciones-varios.component.scss']
})
export class FormProposicionesVariosComponent {
  
  @Input() objetoComiteTecnico: ComiteTecnico 
  @Output() semaforo: EventEmitter<string> = new EventEmitter();
  listaMiembros: Dominio[] = [];


  addressForm = this.fb.group({
    tema: this.fb.array([])
  });

  responsablesArray = [
    { name: 'reponsable 1', value: '1' },
    { name: 'reponsable 2', value: '2' },
    { name: 'reponsable 3', value: '3' }
  ];

  constructor(
              private fb: FormBuilder,
              public dialog: MatDialog,
              private commonService: CommonService,
              private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
              private router: Router,
              private techicalCommitteeSessionService: TechnicalCommitteSessionService,

             ) 
  {

  }

  ngOnInit(): void {
    this.commonService.listaMiembrosComiteTecnico()
      .subscribe( response => {
        this.listaMiembros = response;
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
        ,
      ]],
    });
  }

  onSubmit() {

    console.log(this.addressForm)

    let temas: SesionComiteTema[] = []

    if (this.addressForm.valid) {
      this.tema.controls.forEach( control => {
        let sesionComiteTema: SesionComiteTema = {
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value.codigo,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,
          sesionTemaId: control.get('sesionTemaId').value,
          comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
          esProposicionesVarios: true,

        }
  
        temas.push( sesionComiteTema );
      });

      this.fiduciaryCommitteeSessionService.createEditSesionComiteTema( temas )
        .subscribe( respuesta => {
          this.openDialog('Comité Técnico', respuesta.message)
          if ( respuesta.code == "200" )
            this.validarCompletos(respuesta.data);
        })

    }else{
      this.openDialog('', 'Falta registrar información.')
    }
  }

  validarCompletos(comite: ComiteTecnico) {

    let completo = true;
    let cantidadIncompletos = 0;
    let lista = comite.sesionComiteTema.filter(t => t.esProposicionesVarios);

    lista.forEach(tema => {

      if (tema.tema == undefined || tema.tema.length == 0) {
        completo = false;
        cantidadIncompletos++;

      }
      if (tema.responsableCodigo == undefined || tema.responsableCodigo.length == 0) {
        completo = false;
        cantidadIncompletos++;
      }
      if (tema.tiempoIntervencion == undefined || tema.tiempoIntervencion == 0) {
        completo = false;
        cantidadIncompletos++;
      }

    })

    if (lista.length == 1 && cantidadIncompletos == 3) {
      this.semaforo.emit('sin-diligenciar');
    }
    else if (!completo) {
      this.semaforo.emit('en-proceso');
    }
    else if (completo) {
      this.semaforo.emit('completo');
    }

    console.log(cantidadIncompletos)
  }

  eliminarTema(i) {
    let tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '¿Está seguro de eliminar este registro?', i, tema);

  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result===true) {
        this.deleteTema(e)
      }
    });
  }

  deleteTema(i) {
    let grupo = this.addressForm.get('tema') as FormArray;
    let tema = grupo.controls[i];

    console.log(tema)

    this.techicalCommitteeSessionService.deleteSesionComiteTema(tema.get('sesionTemaId').value)
      .subscribe(respuesta => {
        this.borrarArray(grupo, i)
        this.openDialog('', '<b>La información se ha eliminado correctamente.</b>')
        this.ngOnInit();
      })

  }

  cargarRegistros(){

    this.validarCompletos(this.objetoComiteTecnico);
    let lista = this.objetoComiteTecnico.sesionComiteTema.filter( t => t.esProposicionesVarios )

    lista.forEach( te => {
      let grupoTema = this.crearTema();
      let responsable = this.listaMiembros.find( m => m.codigo == te.responsableCodigo )

      grupoTema.get('tema').setValue( te.tema );
      grupoTema.get('responsable').setValue( responsable );
      grupoTema.get('tiempoIntervencion').setValue( te.tiempoIntervencion );
      grupoTema.get('url').setValue( te.rutaSoporte );
      grupoTema.get('sesionTemaId').setValue( te.sesionTemaId );


      this.tema.push( grupoTema )
    })

  }
}
