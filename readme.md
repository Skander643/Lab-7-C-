<div class="col-md-4">
    <div class="card shadow-sm h-100">
        <div class="card-header bg-white">
            <h5 class="mb-0"><i class="bi bi-speedometer2 text-danger"></i> Max Detected Value</h5>
        </div>
        <div class="card-body text-center d-flex flex-column justify-content-center">
            
            <RadzenRadialGauge Style="width: 100%; height: 250px;">
                <RadzenRadialGaugeScale StartAngle="0" EndAngle="100" Step="20">
                    <RadzenRadialGaugeScalePointer Value="@Max" Length="0.6" ShowValue="true" />
                    <!-- Green zone -->
                    <RadzenRadialGaugeScaleRange From="0" To="40" Fill="green" />
                    <!-- Orange zone -->
                    <RadzenRadialGaugeScaleRange From="40" To="70" Fill="orange" />
                    <!-- Red zone -->
                    <RadzenRadialGaugeScaleRange From="70" To="100" Fill="red" />
                </RadzenRadialGaugeScale>
            </RadzenRadialGauge>

        </div>
    </div>
</div>