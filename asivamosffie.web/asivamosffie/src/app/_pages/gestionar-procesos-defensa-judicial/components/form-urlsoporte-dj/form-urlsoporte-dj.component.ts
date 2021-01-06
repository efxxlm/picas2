import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
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
      console.log(this.defensaJudicial.urlSoporteProceso);
      this.addressForm.get("urlSoporte").setValue(this.defensaJudicial.urlSoporteProceso);  
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
          if(id>0 && this.defensaJudicial.defensaJudicialId==0)
          {
            this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
          }                  
          else{
            location.reload();
          }
      });
    }
  }

  onSubmit() {
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
    }
    defensaJudicial.urlSoporteProceso=this.addressForm.get("urlSoporte").value;
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );

  }
}
