import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-actuacion-proceso',
  templateUrl: './registrar-actuacion-proceso.component.html',
  styleUrls: ['./registrar-actuacion-proceso.component.scss']
})
export class RegistrarActuacionProcesoComponent implements OnInit {

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
  defensaJudicial: DefensaJudicial={};
  estaEditando = false;
  constructor(  private fb: FormBuilder, public dialog: MatDialog,
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];
      this.judicialServices.GetDefensaJudicialById(this.controlJudicialId).subscribe(respose=>{
        this.defensaJudicial=respose;
        this.addressForm.patchValue(this.defensaJudicial.defensaJudicialSeguimiento[0]);
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
  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
          if(id>0)
          {
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(["/gestionarProcesoDefensaJudicial/actualizarProceso/"+id], {})
            );
          }                  
      });
    }
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let defensaJudicial=this.defensaJudicial;
    
    defensaJudicial.defensaJudicialSeguimiento.push({
      defensaJudicialSeguimientoId: this.addressForm.get("defensaJudicialSeguimientoId").value,
      defensaJudicialId: this.controlJudicialId,
      estadoProcesoCodigo:this.addressForm.get("estadoAvanceProceso").value != null ? this.addressForm.get("estadoAvanceProceso").value.codigo : null,
      actuacionAdelantada:this.addressForm.get("actuacionAdelantada").value,
      proximaActuacion:this.addressForm.get("proximaActuacionRequerida").value,
      fechaVencimiento:this.addressForm.get("fechaVencimientoTerminos").value,
      esRequiereSupervisor:this.addressForm.get("actuacionParticipaSupervisor").value,
      observaciones:this.addressForm.get("observaciones").value,
      esprocesoResultadoDefinitivo:this.addressForm.get("actuacionDefinitiva").value,
      rutaSoporte:this.addressForm.get("urlSoporte").value
    });
      console.log(defensaJudicial);
      this.judicialServices.createOrEditDefensaJudicialSeguimiento(defensaJudicial.defensaJudicialSeguimiento[0]).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );
  }
}
