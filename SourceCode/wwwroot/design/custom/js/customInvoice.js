$(document).ready(function () {

    $("#TextSpan").css({ "display": "none" });
    $(document).on("change", ".product-ddl", function () {
        var CurrentItem = $(this);
        // Current element saved in item for future as ajax requesrt will change the meaning of this
        if (CurrentItem.val()) {
            $.ajax({
                type: "GET",
                url: "/api/Common/SingleProduct/" + CurrentItem.val(),
                success: function (data) {
                    var Product = jQuery.parseJSON(JSON.stringify(data.Product));
                    // parse product to json

                    CurrentItem.closest('tr').find('td .product-price').val(JSON.stringify(Product.ProductUnitPrice));

                    // If quantity is present than gain calculate price
                    var PriceOfItem = JSON.stringify(Product.ProductUnitPrice);
                    if (CurrentItem.closest('tr').find('td .product-quantity').val()) {
                        var PricePerUnit = parseFloat(PriceOfItem);
                        var SubTotal = PricePerUnit * parseFloat(CurrentItem.closest('tr').find('td .product-quantity').val());
                        CurrentItem.closest('tr').find('td .product-subtotal').val(SubTotal);
                        //calulate total when product changes
                        CalulateTotal();
                        //calulate taxes when product changes
                        CalulateTaxes($("#TaxCheckbox"));
                    }

                },
                error: function () {
                    alert('Sorry there is some error, please try later.');
                }

            });


        }

    });

    $(document).on("blur", ".product-quantity", function () {
        var CurrentItem = $(this);
        var PricePerUnit = parseFloat(CurrentItem.closest('tr').find('td .product-price').val());
        var SubTotal = PricePerUnit * CurrentItem.val();
        CurrentItem.closest('tr').find('td .product-subtotal').val(SubTotal);
        CalulateTotal();
        // Set Data to send to backend
        var DataToSet = CurrentItem.closest('tr').find('td .product-ddl').val() + "-" + CurrentItem.val();

        CurrentItem.closest('tr').find('td .Pid-Quant').val(DataToSet);

        // for taxes
        var chkbox = $("#TaxCheckbox");
        CalulateTaxes(chkbox);

    });

    $(document).on("click", "#AddNewRow", function () {
        if (ChekAllDetailsFilled()) {

            $("#ProductDetails tbody").append($("#rowHtml").html());

        }
        else {
            alert('Please fill all details of earlier product.');
        }
    });

    $(document).on("click", "#remove-row-link", function () {

        $(this).closest('tr').remove();

        // calulate final total while removing
        CalulateTotal();
    });

    //not allow non numeric values
    $(".product-quantity").keyup(function () {
        var $this = $(this);
        $this.val($this.val().replace(/[^\d.]/g, ''));
    });

    function CalulateTotal() {
        var Total = parseFloat(0);
        $(".product-subtotal").each(function () {
            var SubTotal = $(this).val();
            Total = Total + parseFloat(SubTotal);
        });
        $("#TotalWithoutTax").val(Total);
    }

    function ChekAllDetailsFilled() {
        var result = true;
        $("#ProductDetails .product-ddl option:selected").each(function () {
            var ProductName = $(this).val();
            // null , 0, undefined all are false
            if (!ProductName) {
                result = false;
            }

        });
        $("#ProductDetails .product-price").each(function () {
            var ProductPrice = $(this).val();
            // null , 0, undefined all are false
            if (!ProductPrice) {
                result = false;
            }

        });
        $("#ProductDetails .product-quantity").each(function () {
            var ProductQnt = $(this).val();
            // null , 0, undefined all are false
            if (!ProductQnt) {
                result = false;
            }

        });
        $("#ProductDetails .product-subtotal").each(function () {
            var ProductST = $(this).val();
            // null , 0, undefined all retuns false
            if (!ProductST) {
                result = false;
            }

        });
        return result;

    }

    $(document).on("change", "#TaxCheckbox", function () {
        CalulateTaxes($(this));
    });
    function CalulateTaxes(itemPass) {

        var CurrentItem = itemPass.is(':checked');
        if (CurrentItem) {
            $("#TextSpan").css({ "display": "block" });
            var PrecentageOfTax = parseFloat($("#taxPrecntage").text());
            if (PrecentageOfTax) {
                var TotalWOTax = parseFloat($("#TotalWithoutTax").val());
                var TaxCalc = (PrecentageOfTax / 100) * TotalWOTax;
                $("#TotalTaxes").val(TaxCalc.toFixed(2));
                var GrandTotal = parseFloat(TotalWOTax) + parseFloat(TaxCalc);
                $("#TotalWithTaxes").val(GrandTotal.toFixed(2));
            }

        }
        else {
            $("#TextSpan").css({ "display": "none" });
            $("#TotalWithTaxes").val('0');
            $("#TotalTaxes").val('0');
        }
    }
    $(document).on("change", "#customer-ddl", function () {
        var CurrentItem = $(this);
        if (CurrentItem.val()) {
            $.ajax({
                type: "GET",
                url: "/api/Common/SingleCustomer/" + CurrentItem.val(),
                success: function (data) {
                    var Customer = jQuery.parseJSON(JSON.stringify(data.Customer));
                    // parse Customer to json
                    $("#CustName").text(Customer.Name);
                    $("#CustEmail").text("Email: " + Customer.Email);
                    $("#CustAddress").text(Customer.Address);
                    $("#CustPhone").text("Ph: " + Customer.Phone);

                },
                error: function () {
                    alert('Sorry there is some error , please try later.');
                }

            });


        }
    });
    $(document).on("click", ".btn-validate", function () {
        if (ChekAllDetailsFilled()) {

            return true;

        }
        else {
            alert('fill all fields');
            return false;
        }
    });

    $(document).on("blur", "#SpecificNotes", function () {
        var data = $(this).val();
        if (data) {
            $("SpecificNotesDispalyHeader").html("Important Notes: <br/>");
            $("#SpecificNotesDispaly").val(data);
        }
    });

    $(function () {
        $(".datepicker").datepicker();
    });
});
