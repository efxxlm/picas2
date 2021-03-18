import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-urlsoporte-dj',
  templateUrl: './form-urlsoporte-dj.component.html',
  styleUrls: ['./form-urlsoporte-dj.component.scss']
})
export class FormUrlsoporteDjComponent implements OnInit {

 
  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  estaEditando = false;
  constructor(  private fb: FormBuilder,public commonService:CommonService,
    public defensaService:DefensaJudicialService,
    public dialog: MatDialog, private router: Router  ) { }

  ngOnInit(): void {
  }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  cargarRegistro() {
    //this.ngOnInit().then(() => {
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      console.log(this.defensaJudicial.urlSoporteProceso);
      this.addressForm.get("urlSoporte").setValue(this.defensaJudicial.urlSoporteProceso);  
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () =>   this.router.navigate(
                        [
                            '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                            id
                        ]
                    )
          );
        }
        else {
          if(this.defensaJudicial.defensaJudicialId == id){
            this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
              () =>   this.router.navigate(
                          [
                              '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                              id
                          ]
                      )
            );
          }else{
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let defensaJudicial=this.defensaJudicial;
    if(!this.defensaJudicial.defensaJudicialId||this.defensaJudicial.defensaJudicialId==0)
    {
      defensaJudicial={
        defensaJudicialId:this.defensaJudicial.defensaJudicialId,
        //legitimacionCodigo:this.legitimacion,
        tipoProcesoCodigo:this.tipoProceso,
        //cantContratos:this.formContratista.get( 'numeroContratos' ).value,
        esLegitimacionActiva:this.legitimacion,
        
      };
    }else{
      this.tipoProceso != null ? defensaJudicial.tipoProcesoCodigo = this.tipoProceso : this.defensaJudicial.tipoProcesoCodigo;
      this.legitimacion != null ? defensaJudicial.esLegitimacionActiva = this.legitimacion : this.defensaJudicial.esLegitimacionActiva;
    }
    defensaJudicial.urlSoporteProceso=this.addressForm.get("urlSoporte").value;
    if(this.tipoProceso==null || this.legitimacion==null){
      this.openDialog('', '<b>Falta registrar información.</b>');
    }
    else{
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial)
      .subscribe((response: Respuesta) => {
        this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
      },
      err => {
        this.openDialog('', err.message);
      });
    }
  }
}