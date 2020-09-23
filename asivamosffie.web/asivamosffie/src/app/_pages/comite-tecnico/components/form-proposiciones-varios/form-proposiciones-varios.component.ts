import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ComiteTecnico, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-proposiciones-varios',
  templateUrl: './form-proposiciones-varios.component.html',
  styleUrls: ['./form-proposiciones-varios.component.scss']
})
export class FormProposicionesVariosComponent implements OnInit {

  @Input() objetoComiteTecnico: ComiteTecnico 
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
              private technicalCommitteSessionService: TechnicalCommitteSessionService,
              private router: Router

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

      this.technicalCommitteSessionService.createEditSesionComiteTema( temas )
        .subscribe( respuesta => {
          this.openDialog('Comité Técnico', respuesta.message)
          if ( respuesta.code == "200" )
            this.router.navigate(['/comiteTecnico/registrarSesionDeComiteTecnico',this.objetoComiteTecnico.comiteTecnicoId])
        })

    }else{
      this.openDialog('', 'Falta registrar información.')
    }
  }

  cargarRegistros(){

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
