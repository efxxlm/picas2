import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialSeguimiento, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-ver-detalle-editar-actuacion-proceso',
  templateUrl: './ver-detalle-editar-actuacion-proceso.component.html',
  styleUrls: ['./ver-detalle-editar-actuacion-proceso.component.scss']
})
export class VerDetalleEditarActuacionProcesoComponent implements OnInit, OnDestroy {
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
    defensaJudicialSeguimientoId: [null, Validators.required],
    defensaJudicialId: [null, Validators.required],
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
  defensaJudicialId: number;
  //defensaJudicial: DefensaJudicial={};
  defensaJudicial: any;
  realizoPeticion: boolean = false;
  estaEditando = false;
  constructor(  private fb: FormBuilder, public dialog: MatDialog,
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute, private router: Router) {
      this.commonServices.getEstadoAvanceProcesosDefensa().subscribe(
        response=>{
          this.estadoAvanceProcesoArray=response;
        }
      );
     }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      console.log(this.controlJudicialId);
      this.judicialServices.getDefensaJudicialSeguimiento(this.controlJudicialId).subscribe(respose=>{
        this.defensaJudicial=respose;
        console.log(this.defensaJudicial, respose);
        this.defensaJudicialId = this.defensaJudicial.defensaJudicialId;
        this.addressForm.get('defensaJudicialSeguimientoId').setValue(this.defensaJudicial.defensaJudicialSeguimientoId);
        this.addressForm.get('defensaJudicialId').setValue(this.defensaJudicial.defensaJudicialId);
        for (let i = 0; i < this.estadoAvanceProcesoArray.length; i++) {
          const estadoProcesoSelected = this.estadoAvanceProcesoArray.find(t => t.codigo === this.defensaJudicial.estadoProcesoCodigo);
          this.addressForm.get('estadoAvanceProceso').setValue(estadoProcesoSelected);
        }
        this.addressForm.get('actuacionAdelantada').setValue(this.defensaJudicial.actuacionAdelantada);
        this.addressForm.get('proximaActuacionRequerida').setValue(this.defensaJudicial.proximaActuacion);
        this.addressForm.get('fechaVencimientoTerminos').setValue(this.defensaJudicial.fechaVencimiento);
        this.addressForm.get('actuacionParticipaSupervisor').setValue(this.defensaJudicial.esRequiereSupervisor);
        this.addressForm.get('observaciones').setValue(this.defensaJudicial.observaciones);
        this.addressForm.get('actuacionDefinitiva').setValue(this.defensaJudicial.esprocesoResultadoDefinitivo);
        this.addressForm.get('urlSoporte').setValue(this.defensaJudicial.rutaSoporte);
      });
      /*this.judicialServices.getDefensaJudicialSeguimiento(this.controlJudicialId).subscribe(respose=>{
        console.log("Estaaa: ", respose);
        this.defensaJudicial = respose;
      });*/
    });
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  }
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };
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
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(['/gestionarProcesoDefensaJudicial/actualizarProceso/'+this.defensaJudicialId]);
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    // console.log(this.addressForm.value);

    const defensaJudicial: DefensaJudicialSeguimiento = {
      defensaJudicialSeguimientoId: this.addressForm.get("defensaJudicialSeguimientoId").value,
      defensaJudicialId: this.addressForm.get("defensaJudicialId").value,
      estadoProcesoCodigo:this.addressForm.get("estadoAvanceProceso").value != null ? this.addressForm.get("estadoAvanceProceso").value.codigo : null,
      actuacionAdelantada:this.addressForm.get("actuacionAdelantada").value,
      proximaActuacion:this.addressForm.get("proximaActuacionRequerida").value,
      fechaVencimiento:this.addressForm.get("fechaVencimientoTerminos").value,
      esRequiereSupervisor:this.addressForm.get("actuacionParticipaSupervisor").value,
      observaciones:this.addressForm.get("observaciones").value,
      esprocesoResultadoDefinitivo:this.addressForm.get("actuacionDefinitiva").value,
      rutaSoporte:this.addressForm.get("urlSoporte").value
    };
    console.log(this.addressForm.get("estadoAvanceProceso"));
    console.log("Modelo: ",defensaJudicial);
    this.judicialServices.createOrEditDefensaJudicialSeguimiento(defensaJudicial)    
    .subscribe((respuesta: Respuesta) => {
        this.realizoPeticion = true;
        this.openDialog('', respuesta.message);
        //this.ngOnInit();
        return; 
      },
      err => {
        this.openDialog('', err.message);
        this.ngOnInit();
        return;
      });
    //this.openDialog('', 'La información ha sido guardada exitosamente.');
  }

}
