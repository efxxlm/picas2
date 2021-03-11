import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-ver-detalle-editar-actuacion-proceso',
  templateUrl: './ver-detalle-editar-actuacion-proceso.component.html',
  styleUrls: ['./ver-detalle-editar-actuacion-proceso.component.scss']
})
export class VerDetalleEditarActuacionProcesoComponent implements OnInit {

  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
 
  addressForm = this.fb.group({
    estadoAvanceProceso: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    actuacionParticipaSupervisor: [null, Validators.required],
    observaciones: [null, Validators.required],
    actuacionDefinitiva: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  estadoAvanceProcesoArray = [
  ];
  controlJudicialId: any;
  defensaJudicial: DefensaJudicial={};
  estaEditando = false;
  constructor(  private fb: FormBuilder, public dialog: MatDialog,
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];    
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.judicialServices.GetDefensaJudicialById(this.controlJudicialId).subscribe(respose=>{
        this.defensaJudicial=respose;
      });
    });
    this.commonServices.getEstadoAvanceProcesosDefensa().subscribe(
      response=>{
        this.estadoAvanceProcesoArray=response;
      }
    );
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    // console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }

}
